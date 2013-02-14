class Account < ActiveRecord::Base
  attr_accessible :computer_id, :allotted_time, :domain, :tracking, :used_time, :username

  validates :username, :presence => true
  validates :computer_id, :presence => true
  
  belongs_to :computer

  has_many :account_history, :dependent => :destroy
  has_many :account_process, :dependent => :destroy
  has_many :account_program, :dependent => :destroy

  accepts_nested_attributes_for :account_history, :account_process, :account_program

  def as_json options={}
    {
      id: id,
      computer_id: computer_id,
      domain: domain,
      username: username,
      tracking: tracking,
      
      account_history: account_history,
      account_program: account_program,
      account_process: account_process,

      created_at: created_at,
      update_at: updated_at

    }
  end
end
