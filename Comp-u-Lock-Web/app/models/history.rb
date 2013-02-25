class History < ActiveRecord::Base
  attr_accessible :computer_id, :title, :url, :visit_count

  validates :computer_id, presence: true
  validates :url, presence: true, uniqueness: {scope: :computer_id}
  
  belongs_to :computer

  def as_json options={}
    {
      id: id,
      computer_id: computer_id,
      title: title,
      url: url,
      visit_count: visit_count,

      created_at: created_at,
      update_at: updated_at
    }
  end
end
