class Account < ActiveRecord::Base
  attr_accessible :computer_id, :allotted_time, :domain, 
  :tracking, :used_time, :username, :admin,  :account_process_attributes, :restriction_attributes

  validates :username, :presence => true
  validates :computer_id, :presence => true
  
  belongs_to :computer

  
  has_many :account_process, :class_name => "AccountProcess", :dependent => :destroy
  

  has_many :restriction, dependent: :destroy

  accepts_nested_attributes_for :account_process, :restriction

  def as_json options={}
    {
      id: id,
      computer_id: computer_id,
      domain: domain,
      username: username,
      tracking: tracking,
      admin: admin,
      allotted_time: allotted_time,
      used_time: used_time,
      
      account_process_attributes: account_process,

      restriction_attributes: restriction,

      created_at: created_at,
      update_at: updated_at
    }
  end
end
